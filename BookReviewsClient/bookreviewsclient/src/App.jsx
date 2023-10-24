import './App.css';
import { useEffect, useState} from 'react';
import UserLoginStateContext from './Components/UserLoginStateContext';
import BooksQueryForm from './Components/BooksQueryForm';
import LoginForm from './Components/LoginForm';
import NavigationBar from './Components/NavigationBar';
import {BrowserRouter, Routes, Route} from 'react-router-dom';
import Book from './Components/Book';
import RegistrationForm from './Components/Register';
import UserProfile from './Components/UserProfile';

function App() {
  const [userName, setLoggedIn] = useState(null);
  const loggedInState = {userName, setLoggedIn}
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
                book?<Book key={book.id} props={book}/>:null
            }/>
            <Route path="/registration" element={
              <RegistrationForm />
            }>
            </Route>

            <Route path="/profile" element={
              <UserProfile />
            }>
            </Route>
        </Routes>
        </div>
      </BrowserRouter>
    </UserLoginStateContext.Provider>

  )
}

export default App;
