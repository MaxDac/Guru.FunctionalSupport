namespace Com.Guru.FunctionalSupport

module Results =
    let notExistentException = exn "Getting error value"
    
    type Result<'a, 'e> with
        member this.isOk
            with get () =
                match this with
                | Ok _ -> true
                | _ -> false
            
        member this.isError with get () = not (this.isOk)
        
        static member get x =
            match x with
            | Ok v -> v
            | Error _ -> raise notExistentException
        
        static member bind f x =
            match x with
            | Ok v -> f v
            | e -> e
            
        static member unit x = Ok x
           
        member this.map f x = 
            Result.bind (f >> Result.unit) x