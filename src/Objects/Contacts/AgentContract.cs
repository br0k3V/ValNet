namespace ValNet.Objects.Contacts;

public class AgentContract
{
    public static Dictionary<string, string> AgentContacts = new Dictionary<string, string>()
    {
        {"Brimstone", "ace2bb52-de25-45b5-8e11-3dd2088f914d"}, // 01
        {"Viper", "707eab51-4836-f488-046a-cda6bf494859"}, // 02
        {"Omen", "8e253930-4c05-31dd-1b6c-968525494517"}, // 03 
        {"Killjoy", "1e58de9c-4950-5125-93e9-a0aee9f98746"}, // 04
        {"Cypher", "117ed9e3-49f3-6512-3ccf-0cada7e3823b"}, // 05
        {"Sova", "3051fb18-9240-4bf3-a9f5-eb9ae954cd9d"}, // 06
        {"Sage", "569fdd95-4d10-43ab-ca70-79becc718b46"}, // 07
        // agent 08 is lore, in simple terms this agent was in the valorant protocol but took a break if i remember correctly
        {"Phoenix", "eb93336a-449b-9c1b-0a54-a891f7921d69"}, // 09
        {"Jett", "add6443a-41bd-e414-f6ad-e58d267f4e95"}, // 10
        {"Reyna", "a3bfb853-43b2-7238-a4f1-ad90e9e46bcc"}, // 11
        {"Raze", "f94c3b30-42be-e959-889c-5aa313dba261"}, // 12
        {"Breach", "5f8d3a7f-467b-97f3-062c-13acf203c006"}, // 13
        {"Skye", "6f2a04ca-43e0-be17-7f36-b3908627744d"}, // 14
        {"Yoru", "7f94d92c-4234-0a36-9646-3a87eb8b5c89"}, // 15
        {"Astra", "41fb69c1-4189-7b37-f117-bcaf1e96f1bf"}, // 16
        {"KAYO", "601dbbe7-43ce-be57-2a40-4abd24953621"}, // 17
        {"Chamber", "22697a3d-45bf-8dd7-4fec-84a9e28c69d7"}, // 18
        {"Neon", "bb2a4828-46eb-8cd1-e765-15848195d751"}, // 19
        {"Fade", "dade69b4-4f5a-8528-247b-219e5a1facd6"}, // 20
        {"Harbor", "95b78ed7-4637-86d9-7e41-71ba8c293152"}, // 21
        {"Gekko", "e370fa57-4757-3604-3648-499e1f642d3f"}, // 22
        {"Deadlock", "cc8b64c8-4b25-4ff9-6e7f-37b4da43d235"}, // 23
        {"Iso", "0e38b510-41a8-5780-5e8f-568b2a4f2d6c"}, // 24
        {"Clove", "1dbf2edd-4729-0984-3115-daa5eed44993"}, // 25
        {"Vyse", "efba5359-4016-a1e5-7626-b1ae76895940"}, // 26
        {"Tejo", "b444168c-4e35-8076-db47-ef9bf368f384"}, // 27
    };
}

public enum AgentContact
{
    Brimstone,
    Viper,
    Omen,
    Killjoy,
    Cypher,
    Sova,
    Sage,
    Phoenix,
    Jett,
    Reyna,
    Raze,
    Breach,
    Skye,
    Yoru,
    Astra,
    KAYO,
    Chamber,
    Neon,
    Fade,
    Harbor,
    Gekko,
    Deadlock,
    Iso,
    Clove,
    Vyse,
    Tejo
}
