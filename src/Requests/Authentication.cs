﻿using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using ValNet.Objects;
using ValNet.Objects.Authentication;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ValNet.Requests;

public class Authentication : RequestBase 
{
    private const string authUrl = "https://auth.riotgames.com/api/v1/authorization";
    private const string entitleUrl = "https://entitlements.auth.riotgames.com/api/token/v1";
    private const string userInfoUrl = "https://auth.riotgames.com/userinfo";
    private const string regionUrl = "https://riot-geo.pas.si.riotgames.com/pas/v1/product/valorant";
    private const string cookieJson = "{\"client_id\":\"play-valorant-web-prod\",\"nonce\":\"1\",\"redirect_uri\":\"https://playvalorant.com/opt_in" + "\",\"response_type\":\"token id_token\",\"scope\":\"account openid\"}";
    public Authentication(RiotUser pUser) : base(pUser)
    {
        _user = pUser;
    }
    
    public void AuthenticateWithCloud()
    {
        if (_user.loginData.username == null || _user.loginData.password == null)
            throw new Exception("Username or password are empty, please retry when there are values in place.");

        // get initial cookies for auth
        IRestRequest initCookies = new RestRequest(authUrl, Method.POST);
        initCookies.AddJsonBody(cookieJson);
        _user.UserClient.Execute(initCookies);

        // Auth with user details

        var authData = new
        {
            type = "auth",
            username = _user.loginData.username,
            password = _user.loginData.password,
            remember = "true"
        };

        IRestRequest loginRequest = new RestRequest(authUrl, Method.PUT);
        loginRequest.AddJsonBody(JsonSerializer.Serialize(authData));
        var resp = _user.UserClient.Execute(loginRequest);

        if (!resp.IsSuccessful)
            throw new Exception("Failed Login, please check credentials and try again.");

        JToken authObj = JObject.FromObject(JsonConvert.DeserializeObject(resp.Content));
        
        string authURL = authObj["response"]["parameters"]["uri"].Value<string>();

        
        ParseWebToken(authURL);
        _user.UserClient.AddDefaultHeader("Authorization", $"Bearer {_user.tokenData.access}");

        _user.tokenData.entitle = GetEntitlementToken();
        _user.UserClient.AddDefaultHeader("X-Riot-Entitlements-JWT", _user.tokenData.entitle);
        
        _user.UserData = GetUserData();
        _user.UserRegion = GetUserRegion();
        _user.AuthType = AuthType.Cloud;
    }
    
    async void AuthenticateWithSocket()
    {
        _user.AuthType = AuthType.Socket;
    }
    
    public void AuthenticateWithCookies()
    {
        IRestRequest initCookies = new RestRequest(authUrl, Method.POST);
        initCookies.AddJsonBody(cookieJson);
        var resp = _user.UserClient.Execute(initCookies);
        
        if (!resp.IsSuccessful)
            throw new Exception("Failed Login, please check credentials and try again.");

        JToken authObj = JObject.FromObject(JsonConvert.DeserializeObject(resp.Content));

        string authURL = authObj["response"]["parameters"]["uri"].Value<string>();

        
        ParseWebToken(authURL);
        _user.UserClient.AddDefaultHeader("Authorization", $"Bearer {_user.tokenData.access}");

        _user.tokenData.entitle = GetEntitlementToken();
        _user.UserClient.AddDefaultHeader("X-Riot-Entitlements-JWT", _user.tokenData.entitle);
        
        _user.UserData = GetUserData();

        _user.UserRegion = GetUserRegion();
        _user.AuthType = AuthType.Cookie;
    }

    
    void ParseWebToken(string tokenURL)
    {
       _user.tokenData.access = Regex.Match(tokenURL, @"access_token=(.+?)&scope=").Groups[1].Value;
       _user.tokenData.idToken = Regex.Match(tokenURL, @"id_token=(.+?)&token_type=").Groups[1].Value;
    }
    
    private string? GetEntitlementToken()
    {
        RestRequest request = new RestRequest(entitleUrl, Method.POST);
        
        request.AddJsonBody("{}");

        var resp = _user.UserClient.Execute(request);

        if (!resp.IsSuccessful)
            throw new Exception("Failed to get entitlement token.");
        
        var entitlement_token = JObject.FromObject(JsonConvert.DeserializeObject(resp.Content));

        return (entitlement_token["entitlements_token"] ?? throw new InvalidOperationException("Entitlement Token not Found in Request.")).Value<string>();
    }

    private RiotUserData? GetUserData()
    {
        IRestRequest userDataReq = new RestRequest(userInfoUrl, Method.GET);
        var resp = _user.UserClient.Execute(userDataReq);
        if (!resp.IsSuccessful)
            throw new Exception("Failed to get UserData");
        
        return JsonConvert.DeserializeObject<RiotUserData>(resp.Content);

    }

    private RiotRegion GetUserRegion()
    {

        IRestRequest request = new RestRequest(regionUrl, Method.PUT);
        var idTokData = new
        {
            id_token = _user.tokenData.idToken
        };
        request.AddJsonBody(JsonSerializer.Serialize(idTokData));
        var resp = _user.UserClient.Execute(request);
        if (!resp.IsSuccessful)
            throw new Exception("Failed to get user region.");

        JToken authObj = JObject.FromObject(JsonConvert.DeserializeObject(resp.Content));

        string liveRegion = authObj["affinities"]["live"].Value<string>();
        
        switch (liveRegion)
        {
            case"na":
                _user._riotUrl.glzURL = "https://glz-na-1.na.a.pvp.net";
                _user._riotUrl.pdURL = "https://pd.na.a.pvp.net";
                return RiotRegion.NA;
            case "eu":
                _user._riotUrl.glzURL = "https://glz-eu-1.eu.a.pvp.net";
                _user._riotUrl.pdURL = "https://pd.eu.a.pvp.net";
                return RiotRegion.EU;
            case"kr":
                _user._riotUrl.glzURL = "https://glz-kr-1.kr.a.pvp.net";
                _user._riotUrl.pdURL = "https://pd.kr.a.pvp.net";
                return RiotRegion.KR;
            case "latam":
                _user._riotUrl.glzURL = "https://glz-latam-1.na.a.pvp.net";
                _user._riotUrl.pdURL = "https://pd.na.a.pvp.net";
                return RiotRegion.LATAM;
            case"br":
                _user._riotUrl.glzURL = "https://glz-br-1.na.a.pvp.net";
                _user._riotUrl.pdURL = "https://pd.na.a.pvp.net";
                return RiotRegion.BR;
            case "ap":
                _user._riotUrl.glzURL = "https://glz-ap-1.ap.a.pvp.net";
                _user._riotUrl.pdURL = "https://pd.ap.a.pvp.net";
                return RiotRegion.AP;
            default:
                return RiotRegion.NA;

        }
    }
}