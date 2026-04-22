import { useState } from "react";
import type { StockPickAdd } from "../types/StockPickAdd";
import { UsePost } from "../hooks/UsePostStockPick";
import type { PickReason } from "../types/PickReason";


export const AddPickReasonPage = () => {
  const pickReasonStart:PickReason = {
    id: 0,
    stockId: 0,
    reason: ""
  }
  const [pickReasonState, setPickReasonState] = useState(pickReasonStart);

  const onSubmit = async(e: React.MouseEvent)=>{
    e.preventDefault();
    console.log("id:" + pickReasonState.id);
    console.log("stock id:" + pickReasonState.stockId);
    console.log("reason:" + pickReasonState.reason);
    postData('http://localhost:5198/stockpick/1/pickreasons', pickReasonState);
  }

  const postData = async(url:string, pickReason:PickReason)=>{
        try{
            const response = await fetch(url, {
                method:"POST",
                headers: {
                    'Content-Type': 'application/json' // Declare the content type
                },
                body:JSON.stringify(pickReason)
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
    <main className="mt-15 mb-10">
      <h1 className="text-fern">Add Pick Reason</h1>
      <form className="w-100 m-auto">
        <div className="form-div">
          <label htmlFor="pick-id">pick id</label>
          <input 
            type="number"
            id="pick-id"
            placeholder="pick id"
            value={pickReasonState.id}
            onChange={(e)=>setPickReasonState({...pickReasonState, id:parseInt(e.target.value)})}
            className="bg-white border rounded-sm"
          />
        </div>
        <div className="form-div">
          <label htmlFor="stock-id">stock id</label>
          <input 
            type="number"
            id="stock-id"
            placeholder="stock id"
            value={pickReasonState.stockId}
            onChange={(e)=>setPickReasonState({...pickReasonState, stockId:parseInt(e.target.value)})}
            className="bg-white border rounded-sm"
          />
        </div>
        <div className="form-div">
          <label htmlFor="pick-reason">pick reason</label>
          <input 
            type="text"
            id="pick-reason"
            placeholder="pick reason"
            value={pickReasonState.reason}
            onChange={(e)=>setPickReasonState({...pickReasonState, reason:(e.target.value)})}
            className="bg-white border rounded-sm"
          />
        </div>
        <button className="bg-blue-500 text-white text-lg w-24 border rounded-md border-transparent hover:bg-blue-600" onClick={onSubmit}>Submit</button>
        
      </form>
    </main>
  )
}
