import { useState } from "react";
import type { StockPickAdd } from "../types/StockPickAdd";
import { useNavigate } from "react-router";


export const AddStockPick = () => {
  const stockPickStart:StockPickAdd = {
    id: 0,
    stockTicker: "",
    stockBuyDate: "",
    indexTicker: ""
  }
  const [stockPickState, setStockPickState] = useState(stockPickStart);
  const nav = useNavigate();

  const [apiError, setApiError] = useState(false);
  const onSubmit = async(e: React.MouseEvent)=>{
    e.preventDefault();
    postData('http://localhost:5198/stocks', stockPickState);
    
    

  }

  const postData = async(url:string,stockPick:StockPickAdd)=>{
        
    const response = await fetch(url, {
        method:"POST",
        headers: {
            'Content-Type': 'application/json' // Declare the content type
        },
        body:JSON.stringify(stockPick)
    });
    console.log("response");
    console.log(response);
    if (!response.ok) {
        console.log("API POST Error");
        setApiError(true);
    }else{
      const data1 = await response.json();
      setApiError(false);
      nav("/");
    }    
  }
  return (
    <main className="mt-15">
      <h1 className="text-hunter-green">Add Stock Pick</h1>
      {apiError && <div className="mb-4 text-red-600">
        <p>Invalid Stock Pick. Date or Ticker could not be found.</p>
      </div>
      }
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
        <button 
          className="bg-hunter-green text-white text-lg w-40 rounded-md hover:bg-hunter-green-dark hover:cursor-pointer mb-2 disabled:bg-hunter-green-light" 
          onClick={onSubmit}
          disabled = {!stockPickState.stockTicker || !stockPickState.stockBuyDate || !stockPickState.indexTicker}
        >Submit</button>
        
      </form>
    </main>
  )
}
