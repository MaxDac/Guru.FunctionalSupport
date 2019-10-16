namespace Com.Guru.FunctionalSupport

type AsyncResult<'a> = Async<Result<'a, exn>>

module AsyncResult =
    let unwind<'a> (t: AsyncResult<'a>): Async<Result<'a, exn>> = t
    
    let convert<'a, 'e> (t: Async<Result<'a, exn>>): AsyncResult<'a> = t
    
    let unit a = ResultF.unit a |> Async.unit |> convert
    
    let bind (f: 'a -> AsyncResult<'b>) (a: AsyncResult<'a>) =
        async {
            let! result = a
            
            match result with
            | Ok v ->
                return! f v
            | Error e ->
                return Error e
        } |> convert
        
    let map f = bind (f >> unit)
    
    type AsyncResultBuilder() =
        member __.Bind(c, f) = bind f c
        member __.Return(v) = unit v
        member __.ReturnFrom(v) = v
        member __.Combine(e1, e2) = bind (fun () -> e2) e1
