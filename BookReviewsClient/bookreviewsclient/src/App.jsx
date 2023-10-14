import './App.css';
import { useState} from 'react';
import UserLoginStateContext from './Components/UserLoginStateContext';
import BooksQueryForm from './Components/BooksQueryForm';
import LoginForm from './Components/LoginForm';
import NavigationBar from './Components/NavigationBar';
import {BrowserRouter, Routes, Route} from 'react-router-dom';

function App() {
  const [loggedIn, setLoggedIn] = useState(false);
  const loggedInState = {loggedIn, setLoggedIn}


  return (
    <UserLoginStateContext.Provider value={loggedInState}>
      <BrowserRouter>
      <NavigationBar/>
        <Routes>
            <Route path="/login" element={<div className="content-container">
              <LoginForm/>
            </div>}/>
            <Route path="/books" element={
              <div className="content-container">
                <BooksQueryForm/>
              </div>
            }>
           </Route>
        </Routes>
      </BrowserRouter>
    </UserLoginStateContext.Provider>

  )
}

export default App;
