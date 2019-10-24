namespace Com.Guru.FunctionalSupport
open System.Threading.Tasks

type AsyncResult<'a> = Async<Result<'a, exn>>

type 'a asyncResult = AsyncResult<'a>

module AsyncResult =
    let unwind<'a> (t: AsyncResult<'a>): Async<Result<'a, exn>> = t
    
    let convert<'a, 'e> (t: Async<Result<'a, exn>>): AsyncResult<'a> = t
    
    let unit a = Result.unit a |> Async.unit |> convert
    
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
    
    let fromResult r = r |> Async.unit |> convert
    
    let fromTask t = t |> Async.AwaitTask |> convert
    
    let from f =
        async {
            try
                let! result = f()
                return Ok result
            with
            | e -> return Error e
        }
        |> convert
        
    let wrap f =
        async {
            try
                return f() |> Ok
            with
            | e -> return Error e
        }
        |> convert
    
    type AsyncResultBuilder() =
        member __.Bind(c, f) = bind f c
        member __.Return(v) = unit v
        member __.ReturnFrom(v) = v
        member __.Zero() = unit ()
        member __.Combine(e1, e2) = bind (fun () -> e2) e1
        member __.TryWith(expr, handler) =
            catch 
