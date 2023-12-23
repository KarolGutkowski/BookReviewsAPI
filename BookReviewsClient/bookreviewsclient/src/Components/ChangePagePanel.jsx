const ChangePagePanel = (props) =>
{
    const {setPageNumber} = props;
    const {currentPageNum, totalPagesCount} = props;

    function setPageNumberToButtonValue(event) {
        const target = event.target;
        let value = parseInt(target.value);

        if(isNaN(value))
            return;

        if(value >= totalPagesCount)
            value = totalPagesCount;
        else if(value <= 0)
            value = 1;

        target.value = null;
        setPageNumber(value);
    }
        
    const buttons = [];
    if(totalPagesCount > 0 && totalPagesCount <= 3)
    {
        for(let i=1;i<=totalPagesCount;i++)
        {
            buttons.push(createChangePageButtonWithNumber(i));
        }

    }else{
        buttons.push(<button onClick={()=>setPageNumber(1)} key={1} className="change-page-buttons">{1}</button>);
        if(currentPageNum === 1)
        {
            buttons.push(createPageNumberInput());

            if(totalPagesCount > 1) {
                buttons.push(createChangePageButtonWithNumber(totalPagesCount));
            }
        }else{

            if(currentPageNum > 2) {
                buttons.push(createPageNumberInput());
            }

            buttons.push(createChangePageButtonWithNumber(currentPageNum));

            if(currentPageNum < totalPagesCount - 1) {
                buttons.push(createPageNumberInput());
            }

            if(totalPagesCount > 1 && currentPageNum < totalPagesCount) {
                buttons.push(createChangePageButtonWithNumber(totalPagesCount));
            }
        }
    }

    function createPageNumberInput() {
        return (
            <div className="change-page-buttons">
                    <input type="number" onBlur={setPageNumberToButtonValue} placeholder="..." className="any-page-number-input"/>
            </div>
        )
    }

    function createChangePageButtonWithNumber(number) {
        let classes = "change-page-buttons";

        if(number === currentPageNum)
        {
            classes += " current-page-button";
        }

        return (<button onClick={()=>setPageNumber(number)} key={number} className={classes}>{number}</button>);
    }

    return (
    <div className="switch-page-section">
        <button onClick={()=>setPageNumber(currentPageNum-1)} disabled={currentPageNum-1<=0} className="change-page-buttons" >{"<"}</button>
        {buttons}
        <button onClick={()=>setPageNumber(currentPageNum+1)} disabled={currentPageNum+1>totalPagesCount} className="change-page-buttons">{">"}</button>
    </div>);
}


export default ChangePagePanel;