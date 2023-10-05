const Book = ({props})=>
{
    return (
        <div>
            <hr></hr>
            <p>Title: {props.title}</p>
            <p>Year: {props.year}</p>
            <img src={props.img} alt="book cover" height="100px" width="70px"></img>
        </div>
    );
}

export default Book;