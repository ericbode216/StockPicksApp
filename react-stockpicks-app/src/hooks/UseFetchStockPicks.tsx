import { useEffect, useState } from "react";
import type { StockPick } from "../types/StockPick";


export const UseFetchStockPicks = (url: string) => {
    const [stockPicks, setStockPicks] = useState<StockPick[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<Error | null>(null);
    
    
    useEffect(() =>{
        
        fetchData();
    }, [url]);
    
   
    const fetchData = async()=>{
        console.log('fetching data...');
        setLoading(true);
        try{
            const response = await fetch(url);
            const data1 = await response.json();
            setStockPicks(data1);
        }catch(err:unknown){
            setError(err as Error);
        }
        setLoading(false);
        console.log('finished fetching data');
    }
    return {stockPicks,loading, error, fetchData}
}
