import './styles/App.css';
import { useEffect, useState} from 'react';
import UserLoginStateContext from './Components/UserLoginStateContext';
import BooksQueryForm from './Components/BooksQueryForm';
import LoginForm from './Components/LoginForm';
import NavigationBar from './Components/NavigationBar';
import {BrowserRouter, Routes, Route} from 'react-router-dom';
import Book from './Components/Book';
import RegistrationForm from './Components/Register';
import UserProfile from './Components/UserProfile';
import {config} from "./Constants"
import BookDetails from './Components/BookDetails';
import BooksSearchResults from './Components/BooksSearchResults';
import ErrorBoundary from "./Components/ErrorBoundary"
import AddBookForm from './Components/AddBookForm';

function App() {
  const [user, setLoggedIn] = useState(null);
  const loggedInState = {user, setLoggedIn}
  const [book, setBook] = useState(null);
  const [searchResultBooks, setSearchResultBooks] = useState(null);

  useEffect(()=>
  {
    fetch(`${config.url}/api/v1/books/random`,
    {
        credentials: "include",
    })
    .then(response =>response.json())
    .catch(error => console.error("Can't connect to server. Try again later :("))
    .then(data => setBook(data))
  },[])

  return (
    <ErrorBoundary>
    <UserLoginStateContext.Provider value={loggedInState}>
      <BrowserRouter>
      <NavigationBar setSearchResultBooks={setSearchResultBooks}/>
      <div className="content-container">
        <Routes>
            <Route path="/login" element={
                <LoginForm/>
                }/>
            <Route path="/books" element={
                <BooksQueryForm/>
            }>
           </Route>
           <Route path="/" element = {
                <div className='main-page-content-container'>
                  {book?<Book key={book.id} book={book}/>:<div className="loader" id="loader-7"></div>}
                </div>
            }/>
            <Route path="/registration" element={
              <RegistrationForm />
            }>
            </Route>

            <Route path="/profile" element={
              <UserProfile />
            }>
            </Route>

            <Route path="/search" element={
              <BooksSearchResults books={searchResultBooks}/>
            }>
            </Route>

            <Route path="/book/:id" element={
                <BookDetails />
                }/>

            <Route path="/book/add" element={
                <AddBookForm />
                }/>
        </Routes>
        </div>
      </BrowserRouter>
    </UserLoginStateContext.Provider>
  </ErrorBoundary>
  )
}

export default App;
