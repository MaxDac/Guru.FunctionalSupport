namespace Com.Guru.FunctionalSupport

type 'a result = Result<'a, exn>

module Result =
    let notExistentException = exn "Getting error value"

    let isOk res =
        match res with
        | Ok _ -> true
        | _ -> false

    let isError res = isOk res |> not

    let someUnit x = Ok x
        
    let unit = Ok

    let get x =
        match x with
        | Ok v -> v
        | Error _ -> raise notExistentException
        
    let toOption r =
        match r with
        | Ok v -> Some v
        | _ -> None
        
    let flatten r =
        let rec flattenInternal items acc =
            match items with
            | [] -> acc
            | x :: xs ->
                match x with
                | Ok v ->
                    acc
                    |> Result.map (fun ls -> List.append ls [ v ])
                    |> flattenInternal xs 
                | Error e ->
                    Error e
        
        Ok [] |> flattenInternal r 

    type ResultBuilder() =
        member __.Bind(c, f) = Result.bind f c
        member __.Return(v) = unit v
        member __.ReturnFrom(v) = v
        member __.Combine(e1, e2) = Result.bind (fun () -> e2) e1

