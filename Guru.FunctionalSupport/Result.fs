namespace Com.Guru.FunctionalSupport

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

    type ResultBuilder() =
        member __.Bind(c, f) = Result.bind f c
        member __.Return(v) = unit v
        member __.ReturnFrom(v) = v
        member __.Combine(e1, e2) = Result.bind (fun () -> e2) e1
