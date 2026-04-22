import { useEffect, useState } from "react";
import { StockPickCard } from "../components/StockPickCard";
import type { StockPick } from "../types/StockPick";
import { convertToDate, dateDiffNumDays } from "../utility/dateHelpers";
import type { StockPickAdd } from "../types/StockPickAdd";


export const StockPicksList = () => {
  const [stockPicks, setStockPicks] = useState<StockPick[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<Error | null>(null);
  const [stockPicksShown, setStockPicksShown]= useState([...stockPicks]);


  const fetchData = async()=>{
      setLoading(true);
      try{
          const response = await fetch('http://localhost:5198/stocks');
          const data1 = await response.json();
          setStockPicks(data1);
          setStockPicksShown(data1);
      }catch(err:unknown){
          setError(err as Error);
      }
      setLoading(false);
  }


  const updateStock = async(stockPick:StockPick)=>{
        try{
            const stockPickUpdate:StockPickAdd ={
                id: stockPick.id,
                stockTicker: stockPick.stockTicker,
                stockBuyDate: stockPick.stockBuyDate,
                indexTicker: stockPick.indexTicker
            }
            const response = await fetch('http://localhost:5198/stocks', {
                method:"PUT",
                headers: {
                    'Content-Type': 'application/json' // Declare the content type
                },
                body:JSON.stringify(stockPickUpdate)
            });
            const data1 = await response.json();
            fetchData();
            
            if (!response.ok) {
                throw Error(`HTTP error! status: ${response.status}`);
            }
            
        }catch(err:unknown){
            console.log(err as Error);
        }
    }

    const updateAllStocks = async()=>{
      stockPicks.map((stockPick)=>{
        updateStock(stockPick);
      })
    }



    useEffect(()=>{
      fetchData();
    },[])



  if(loading || stockPicks === null){
    return (
      <>
        <h1>StockPicksList</h1>
        <p>Loading...</p>;
      </>
    )
  }
  if(error){
    console.log(error);
  }
  const checkOverEqualNumDays = (date1:Date, date2:Date, numDays: number) =>{
    const days = dateDiffNumDays(date1,date2);
    if(days>=numDays){
      return true;
    }
    return false;
  }
  const filterStocks = (criteria: string ) =>{
    if(criteria === "all"){
      setStockPicksShown(stockPicks);
    }else if(criteria === "oneyear"){
      setStockPicksShown(stockPicks.filter((stockPick)=>checkOverEqualNumDays(convertToDate(stockPick.stockCurrentDate),convertToDate(stockPick.stockBuyDate), 365)));
    }else if(criteria == "threeyear"){
      setStockPicksShown(stockPicks.filter((stockPick)=>checkOverEqualNumDays(convertToDate(stockPick.stockCurrentDate),convertToDate(stockPick.stockBuyDate), 365*3)));
    }
    
  }


  return (
    <main className="mt-15 mb-10">
        <h1 className="text-fern">Stock Picks List</h1>
        <button className="bg-blue-500 text-white text-lg w-24 border rounded-md border-transparent hover:bg-blue-600 m-4" onClick={()=>filterStocks("all")}>All</button>
        <button className="bg-blue-500 text-white text-lg w-24 border rounded-md border-transparent hover:bg-blue-600 m-4" onClick={()=>filterStocks("oneyear")}>Year +</button>
        <button className="bg-blue-500 text-white text-lg w-24 border rounded-md border-transparent hover:bg-blue-600 m-4"onClick={()=>filterStocks("threeyear")}>3 Years +</button>
        <button className="bg-blue-500 text-white text-lg w-24 border rounded-md border-transparent hover:bg-blue-600 m-4"onClick={()=>updateAllStocks()}>Update All</button>
        <div className="justify-center">
        {stockPicksShown.map((stockPick)=>
          <div key={stockPick.id}>
          <StockPickCard stockPick={stockPick} updateStock={updateStock}/>
          </div>
        )}
        </div>
    </main>
  )
}
