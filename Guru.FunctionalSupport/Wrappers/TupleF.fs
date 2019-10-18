namespace Com.Guru.FunctionalSupport.Wrappers

module TupleF =
    let map f t =
        match t with
        | (a, b) -> (f a, f b)
        
    let map3 f t =
        match t with
        | (a, b, c) -> (f a, f b, f c)
