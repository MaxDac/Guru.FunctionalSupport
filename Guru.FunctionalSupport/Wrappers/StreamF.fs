namespace Com.Guru.FunctionalSupport

open System.IO
open System.Text

module StreamF =
    let readStreamAsString(s: Stream) =
        use reader = new StreamReader(s, Encoding.UTF8)
        reader.ReadToEnd()
        
    let readStreamAsStringAsync(s: Stream) = async {
        use reader = new StreamReader(s, Encoding.UTF8)
        let! result = Async.AwaitTask(reader.ReadToEndAsync())
        return result
    }
        
    let tryReadStreamAsStringAsync(s: Stream) =
        async {
            try
                let! result = readStreamAsStringAsync s
                return Ok result
            with
            | e ->
                return Error e
        }
        |> AsyncResult.convert
        
    let mergeStreams (x : Stream) (y : Stream) =
        try
            x.Position <- x.Length
            y.Position <- int64(0)
            y.CopyTo(x, 4096)
            x.Position <- int64(0)
            Ok x
        with
        | e -> Error e
        
    let mergeStreamsAsync (x : Stream) (y : Stream) =
        async {
            try
                x.Position <- x.Length
                y.Position <- int64(0)
                do! Async.awaitPlainTask(y.CopyToAsync(x, 4096))
                x.Position <- int64(0)
                return Ok x
            with
            | e -> return Error e
        }
        |> AsyncResult.convert
