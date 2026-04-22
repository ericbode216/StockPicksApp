export function dateDiffNumDays(date1:Date, date2:Date){
        //timeDiff in milliseconds
        let timeDiff = date1.valueOf()- date2.valueOf();
        
        const days = Math.floor(timeDiff/(1000 * 60 * 60 * 24))
        return days;
    }

export function convertToDate(str: string){
        return new Date(str);
    }