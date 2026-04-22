import { useState } from "react";
import type { StockPickAdd } from "../types/StockPickAdd";
import { parseISO } from "date-fns";
import { UsePost } from "../hooks/UsePostStockPick";
import type { StockPick } from "../types/StockPick";


export const AddStockPick = () => {
  const stockPickStart:StockPickAdd = {
    id: 0,
    stockTicker: "",
    stockBuyDate: "",
    indexTicker: ""
  }
  const [stockPickState, setStockPickState] = useState(stockPickStart);
  const onSubmit = async(e: React.MouseEvent)=>{
    e.preventDefault();
    postData('http://localhost:5198/stocks', stockPickState);
  }

  const postData = async(url:string,stockPick:StockPickAdd)=>{
        try{
            const response = await fetch(url, {
                method:"POST",
                headers: {
                    'Content-Type': 'application/json' // Declare the content type
                },
                body:JSON.stringify(stockPick)
            });
            const data1 = await response.json();
            if (!response.ok) {
                throw Error(`HTTP error! status: ${response.status}`);
            }
            
        }catch(err:unknown){
            console.log(err as Error);
        }
    }
  return (
    <main className="mt-15">
      <h1 className="text-fern">Add Stock Pick</h1>
      <form className="w-100 m-auto">

        <div className="form-div">
          <label htmlFor="stock-ticker">stock ticker</label>
          <input 
            type="text"
            id="stock-ticker"
            placeholder="stock ticker"
            value={stockPickState.stockTicker}
            onChange={(e)=>setStockPickState({...stockPickState, stockTicker:(e.target.value)})}
            className="bg-white border rounded-sm"
          />
        </div>
        <div className="form-div">
          <label htmlFor="stock-buy-date">stock buy date</label>
          <input 
            type="date"
            id="stock-buy-date"
            value={stockPickState.stockBuyDate}
            onChange={(e)=>setStockPickState({...stockPickState, stockBuyDate:e.target.value})}
            className="bg-white border rounded-sm"
          />
        </div>
        <div className="form-div">
          <label htmlFor="index-ticker">index ticker</label>
          <input 
            type="text"
            id="index-ticker"
            placeholder="index ticker"
            value={stockPickState.indexTicker}
            onChange={(e)=>setStockPickState({...stockPickState, indexTicker:(e.target.value)})}
            className="bg-white border rounded-sm"
          />
        </div>
        <button className="bg-blue-500 text-white text-lg w-24 border rounded-md border-transparent hover:bg-blue-600" onClick={onSubmit}>Submit</button>
        
      </form>
    </main>
  )
}
