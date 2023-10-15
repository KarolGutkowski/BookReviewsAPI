import './App.css';
import { useEffect, useState} from 'react';
import UserLoginStateContext from './Components/UserLoginStateContext';
import BooksQueryForm from './Components/BooksQueryForm';
import LoginForm from './Components/LoginForm';
import NavigationBar from './Components/NavigationBar';
import {BrowserRouter, Routes, Route} from 'react-router-dom';
import Book from './Components/Book';

function App() {
  const [loggedIn, setLoggedIn] = useState(false);
  const loggedInState = {loggedIn, setLoggedIn}
  const [book, setBook] = useState(null);

  useEffect(()=>
  {
    fetch("https://localhost:7235/api/v1/books/random",
    {
        credentials: "include",
    })
    .then(response =>response.json())
    .then(data => setBook(data))
  },[])

  return (
    <UserLoginStateContext.Provider value={loggedInState}>
      <BrowserRouter>
      <NavigationBar/>
        <Routes>
            <Route path="/login" element={
              <div className="content-container">
                <LoginForm/>
              </div>}/>
            <Route path="/books" element={
              <div className="content-container">
                <BooksQueryForm/>
              </div>
            }>
           </Route>
           <Route path="/" element = {
              <div className="content-container">
                {book?<Book key={book.id} props={book}/>:null}
              </div>
            }/>
        </Routes>
      </BrowserRouter>
    </UserLoginStateContext.Provider>

  )
}

export default App;
