import {config} from "../Constants";


const AddBookForm = () =>
{
    async function handleBookAddOnSubmit(event) 
    {
        event.preventDefault();
        const form = document.getElementById("book-add-form");
        //const fileInput = document.getElementById("book-cover-input");
       //const file = fileInput[0];
        const formData = new FormData(form);
        
        const result = await fetch(`${config.url}/api/v1/books/add`,{
            method: 'POST',
            credentials: "include",
            body: formData
        });
    }

    return (<>
        <form id="book-add-form" className="add-books-form" onSubmit={handleBookAddOnSubmit}>
            <input type="text" placeholder="Title" name="Title" className="app-input"/>
            <input type="number" placeholder="Year" name="Year" className="app-input"/>
            <textarea placeholder="Description" name="Description" className="app-input"/>
            <input type="text" placeholder="AuthorFirstName" name="AuthorFirstName" className="app-input"/>
            <input type="text" placeholder="AuthorLastName" name="AuthorLastName" className="app-input"/>

            <label htmlFor="BookCover">
                Book cover image:
            </label>
            <input type="file" name="BookCover" className="app-file-input" id="book-cover-input" accept="image/*"/>
            <button>Send</button>
        </form>
    </>);
}

export default AddBookForm;