import { useEffect, useState } from "react";
import type { PickReason } from "../types/PickReason";


export const UseFetchPickReasons = (url: string) => {
    const [data, setData] = useState<PickReason[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<Error | null>(null);
    
    useEffect(() =>{
        
        fetchData();
    }, [url]);

    const fetchData = async()=>{
        setLoading(true);
        try{
            const response = await fetch(url);
            const data1 = await response.json();
            setData(data1);
        }catch(err:unknown){
            setError(err as Error);
        }
        setLoading(false);
    }
    return {data,loading, error}
}