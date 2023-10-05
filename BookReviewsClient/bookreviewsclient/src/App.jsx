import './App.css';
import { useState} from 'react';
import UserLoginStateContext from './Components/UserLoginStateContext';
import BooksQueryForm from './Components/BooksQueryForm';
import LoginForm from './Components/LoginForm';
import { logDOM } from '@testing-library/react';

function App() {
  const [loggedIn, setLoggedIn] = useState(false);
  const loggedInState = {loggedIn, setLoggedIn}


  return (
    <UserLoginStateContext.Provider value={loggedInState}>
      {loggedIn? <BooksQueryForm/>: <LoginForm />}
    </UserLoginStateContext.Provider>
  )
}

export default App;
