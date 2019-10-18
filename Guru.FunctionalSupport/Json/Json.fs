namespace Com.Guru.FunctionalSupport

open Newtonsoft.Json
open Newtonsoft.Json.Serialization

module Json =
    let defaultSettings =
        lazy
        let mutable returnType = new JsonSerializerSettings()
        returnType.ContractResolver <- new CamelCasePropertyNamesContractResolver()
        returnType;
    
    let isString x = box x :? string
    
    let asString x = box x :?> string
    
    let serialize x =
        match (isString x) with
        | true -> asString x
        | false -> JsonConvert.SerializeObject(x, defaultSettings.Value)
    
    let deserialize<'a> x =
        let deserializeInternal() = JsonConvert.DeserializeObject<'a>(x)
        
        try
            deserializeInternal() |> Ok
        with
        | :? JsonException as ex -> Error ex
        