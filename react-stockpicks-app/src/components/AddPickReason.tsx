import { useState } from "react";
import type { StockPickAdd } from "../types/StockPickAdd";
import { UsePost } from "../hooks/UsePostStockPick";
import type { PickReason } from "../types/PickReason";

interface Props {
  stockId: number;
  fetchPickReasons:Function;
}
export const AddPickReason = ({stockId, fetchPickReasons}: Props) => {
  const pickReasonStart:PickReason = {
    id: 0,
    stockId: stockId,
    reason: ""
  }
  const [pickReasonState, setPickReasonState] = useState(pickReasonStart);

  const onSubmit = async(e: React.MouseEvent)=>{
    e.preventDefault();
    console.log("id:" + pickReasonState.id);
    console.log("stock id:" + pickReasonState.stockId);
    console.log("reason:" + pickReasonState.reason);
    await postData('http://localhost:5198/stockpick/1/pickreasons', pickReasonState);
    fetchPickReasons();

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
    <div>
      <h2>Add Pick Reason</h2>
      <form className="w-100 m-auto">
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
      </div>
  )
}
