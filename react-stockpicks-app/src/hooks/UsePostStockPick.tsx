import { useState } from "react";
import type { StockPick } from "../types/StockPick";
import type { StockPickAdd } from "../types/StockPickAdd";


export const UsePost= (url: string, stockPick:StockPickAdd) => {
    const [data, setData] = useState<StockPick[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<Error | null>(null);
    
    

    const postData = async()=>{
        setLoading(true);
        try{
            const response = await fetch(url, {
                method:"POST",
                headers: {
                    'Content-Type': 'application/json' // Declare the content type
                },
                body:JSON.stringify(stockPick)
            });
            const data1 = await response.json();
            setData(data1);
            if (!response.ok) {
                setError(new Error(`HTTP error! status: ${response.status}`));
            }
            
        }catch(err:unknown){
            setError(err as Error);
        }
        setLoading(false);
    }
    postData();
    
    return {data,loading, error}
}