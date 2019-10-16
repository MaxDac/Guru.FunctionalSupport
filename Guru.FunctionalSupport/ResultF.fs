namespace Com.Guru.FunctionalSupport

module ResultF =
    let notExistentException = exn "Getting error value"

    let isOk res =
        match res with
        | Ok _ -> true
        | _ -> false

    let isError res = isOk res |> not

    let someUnit x = Ok x

    let bind f x =
        match x with
        | Ok v -> f v
        | e -> e
        
    let unit = Ok

    let map f x =
        bind (f >> unit) x

    let get x =
        match x with
        | Ok v -> v
        | Error _ -> raise notExistentException

    type ResultBuilder() =
        member __.Bind(c, f) = bind f c
        member __.Return(v) = unit v
        member __.ReturnFrom(v) = v
        member __.Combine(e1, e2) = bind (fun () -> e2) e1

