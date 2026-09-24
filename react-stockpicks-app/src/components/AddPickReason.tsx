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
    reason: "",
    date: new Date().toDateString()
  }
  const [pickReasonState, setPickReasonState] = useState(pickReasonStart);

  const onSubmit = async(e: React.MouseEvent)=>{
    e.preventDefault();
    console.log("id:" + pickReasonState.id);
    console.log("stock id:" + pickReasonState.stockId);
    console.log("reason:" + pickReasonState.reason);
    setPickReasonState({...pickReasonState, date: new Date().toDateString()});
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
            placeholder="add pick reason to submit"
            value={pickReasonState.reason}
            onChange={(e)=>setPickReasonState({...pickReasonState, reason:(e.target.value)})}
            className="bg-white border rounded-sm"
          />
        </div>
        <button className="bg-hunter-green text-white text-lg w-40 rounded-md hover:bg-hunter-green-dark hover:cursor-pointer mb-2 disabled:bg-hunter-green-light" 
                onClick={onSubmit}
                disabled = {!pickReasonState.reason}
        >Submit</button>
      </form>
      </div>
  )
}
