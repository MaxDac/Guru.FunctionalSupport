namespace Com.Guru.FunctionalSupport

module Option =
    let unit o = Some o
    
    let orElseFalse o =
        match o with
        | None -> false
        | Some v -> v
        
    let asResult e o =
        match o with
        | None -> Error e
        | Some v -> Ok v
        
    let joinOptional x xss =
        xss
        |> Option.bind (fun yss ->
            x |> Option.map (fun y -> y :: yss))
    
    let traverseList xs =
        List.foldBack joinOptional xs (Some []) 
            
    type OptionBuilder() =
        member __.Zero() = None
        member __.Return x = unit x
        member __.ReturnFrom x = x
        member __.Bind (x, f) = Option.bind f x
        member __.Combine(e1, e2) = Option.bind (fun () -> e2) e1
