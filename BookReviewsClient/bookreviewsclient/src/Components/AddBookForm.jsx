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
        debugger;
        const result = await fetch(`${config.url}/api/v1/books`,{
            method: 'POST',
            credentials: "include",
            body: formData
        });

        if(result.ok)
        {
            form.reset();
        }
    }

    return (<>
        <form id="book-add-form" className="add-books-form" onSubmit={handleBookAddOnSubmit}>
            <input type="text" placeholder="Title" name="Title" className="app-input" required/>
            <input type="number" placeholder="Year" name="Year" className="app-input" required/>
            <textarea placeholder="Description" name="Description" className="app-input" required/>
            <input type="text" placeholder="AuthorFirstName" name="AuthorFirstName" className="app-input" required/>
            <input type="text" placeholder="AuthorLastName" name="AuthorLastName" className="app-input" required/>

            <label htmlFor="BookCover">
                Book cover image:
            </label>
            <input type="file" name="BookCover" className="app-file-input" id="book-cover-input" accept="image/*" required/>
            <button>Send</button>
        </form>
    </>);
}

export default AddBookForm;