import { useEffect, useState } from "react";
import { UseFetchPickReasons } from "../hooks/UseFetchPickReasons";
import type { StockPick } from "../types/StockPick";
import type { StockPickAdd } from "../types/StockPickAdd";
import { convertToDate, dateDiffNumDays } from "../utility/dateHelpers";
import  { AddPickReason } from "./AddPickReason";
import type { PickReason } from "../types/PickReason";


interface Props {
  stockPick: StockPick;
  updateStock:Function;
}


export const StockPickCard = ({stockPick, updateStock}: Props) => {
    const [pickReasons, setPickReasons] = useState<PickReason[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<Error | null>(null);
    
    const fetchPickReasons = async()=>{
        setLoading(true);
        try{
            const response = await fetch(`http://localhost:5198/stockpick/${stockPick.id}/pickreasons`);
            const data1 = await response.json();
            setPickReasons(data1);
        }catch(err:unknown){
            setError(err as Error);
        }
        setLoading(false);
    }

    useEffect(() =>{     
        fetchPickReasons();
    }, []);

    const [showReasoningForm, setShowReasoningForm] = useState(false);
    if(loading || pickReasons === null){
    return (
      <div key={stockPick.id}>
        <h1>StockPicksList</h1>
        <p>Loading...</p>
      </div>
    )
    }
    if(error){
        console.log(error);
    }
    const dateDifferenceString = (date1:Date, date2:Date) =>{
        
        const days = dateDiffNumDays(date1,date2)
        
        let years = Math.floor(days/365);
        let dateStr = "";
        if(years > 0){
            if(years == 1){
                dateStr += Math.floor(days/365)+" year and ";
                dateStr += days-365 + " days";
                return dateStr;

            }else{
                dateStr += years +" years and ";
                dateStr += days-365*years + " days";
                return dateStr;
            }
        }
        
        return days + " days";
    }
    
  return (
    <div className={`justify-center mb-6 border rounded-md overflow-hidden ${stockPick.stockTotalPercentGain && stockPick.indexTotalPercentGain&& (stockPick.stockTotalPercentGain-stockPick.indexTotalPercentGain)>= 0?"bg-green-100":"bg-red-100"}`}>
        <h2>{stockPick.stockTicker} bought on {convertToDate(stockPick.stockBuyDate).getMonth() +1 }/{convertToDate(stockPick.stockBuyDate).getDate()}/{convertToDate(stockPick.stockBuyDate).getFullYear()}</h2>
        <div className="date-side-div">
            <div className="col-start-2">
                {dateDiffNumDays(convertToDate(stockPick.stockCurrentDate),convertToDate(stockPick.stockBuyDate))> 365?
                <p>annual return diff: {stockPick.stockAnnualPercentGain && stockPick.indexAnnualPercentGain && (stockPick.stockAnnualPercentGain-stockPick.indexAnnualPercentGain).toFixed(2)}%</p>
                :
                ""}
                <p>total return diff: {stockPick.stockTotalPercentGain && stockPick.indexTotalPercentGain && (stockPick.stockTotalPercentGain-stockPick.indexTotalPercentGain).toFixed(2)}%
                    over {dateDifferenceString(convertToDate(stockPick.stockCurrentDate), convertToDate(stockPick.stockBuyDate))}
                </p>
            </div>
            <div className="text-right pr-2">
                <p>last updated: {convertToDate(stockPick.stockCurrentDate).getMonth() +1 }/{convertToDate(stockPick.stockCurrentDate).getDate()}/{convertToDate(stockPick.stockCurrentDate).getFullYear()}</p>
                <button className="bg-hunter-green text-white text-lg w-24 rounded-md hover:bg-hunter-green-dark hover:cursor-pointer mt-2" onClick={()=>updateStock(stockPick)}>Update</button>
            </div>
        </div>
        <br/>
        <div className="grid grid-cols-2">
            {/*left column*/}
            <div>
                <div className="stock-data-div data-div" >
                    <span className="label"> annual return</span>
                    <span className="info">{stockPick.stockAnnualPercentGain && stockPick.stockAnnualPercentGain.toFixed(2)}%</span>
                </div>

                <div className="stock-data-div data-div">
                    <span className="label">stock total return</span>
                    <span className="info">{stockPick.stockTotalPercentGain && stockPick.stockTotalPercentGain.toFixed(2)}%</span>
                </div>
                <br/>
                <div className="stock-data-div data-div">
                    <span className="label">stock buy price</span>
                    <span className="info">${stockPick.stockBuyPrice}</span>
                </div>
                <div className="stock-data-div data-div">
                    <span className="label">stock current price</span>
                    <span className="info">${stockPick.stockCurrentPrice}</span>
                </div>
            </div>
            {/*right column*/}
            <div>
                <div className="index-data-div data-div">
                    <span className="label">index annual return</span>
                    <span className="info">{stockPick.indexAnnualPercentGain && stockPick.indexAnnualPercentGain.toFixed(2)}%</span>
                </div>
                <div className="index-data-div data-div">
                    <span className="label">index total return</span>
                    <span className="info">{stockPick.indexTotalPercentGain && stockPick.indexTotalPercentGain.toFixed(2)}%</span>
                </div>
                <br/>
                <div className="index-data-div data-div">
                    <span className="label">index buy price</span>
                    <span className="info">${stockPick.indexBuyPrice}</span>
                </div>
                <div className="index-data-div data-div">
                    <span className="label">index current price</span>
                    <span className="info">${stockPick.indexCurrentPrice}</span>
                </div>
                <div className="index-data-div data-div">
                    <span className="label">index ticker</span>
                    <span className="info">{stockPick.indexTicker}</span>
                </div>
            </div>
        </div>
        <div className="">
        <h2>Original Thesis:</h2>
        {(pickReasons.length > 0)? 
            <div key={pickReasons[0].id} className="date-side-div m-2 bg-yellow-50">
                <div className="text-left pl-2">
                    <p>{convertToDate(pickReasons[0].date).getMonth() +1 }/{convertToDate(pickReasons[0].date).getDate()}/{convertToDate(pickReasons[0].date).getFullYear()}</p>
                </div>
                <div className="col-start-2">
                    <p>{pickReasons[0].reason}</p>
                </div>
            </div>
            :
            <p>No reasoning entered.</p>
        }
        {(pickReasons.length > 1)?
        <>
        <h2>Updated Reasoning:</h2>
        {pickReasons.toSpliced(0,1).map((p: PickReason)=>
            <div key={p.id} className="date-side-div m-2 bg-yellow-50">
                <div className="text-left pl-2">
                    <p>{convertToDate(p.date).getMonth() +1 }/{convertToDate(p.date).getDate()}/{convertToDate(p.date).getFullYear()}: </p>
                </div>
                <div className="col-start-2">
                    <p>{p.reason}</p>
                </div>
            </div>
            
        )}
        </>
        :
        ""
        }
        </div>
        <div>
            <button className="bg-hunter-green text-white text-lg w-40 rounded-md hover:bg-hunter-green-dark hover:cursor-pointer mb-2" onClick={()=>setShowReasoningForm(!showReasoningForm)}>Add Reasoning</button>
            {(showReasoningForm)?
            <AddPickReason stockId={stockPick.id} fetchPickReasons={fetchPickReasons}/>
            :
            ""
            }
        </div>
    </div>
  )
}
