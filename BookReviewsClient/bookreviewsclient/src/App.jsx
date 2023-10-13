import './App.css';
import { useState} from 'react';
import UserLoginStateContext from './Components/UserLoginStateContext';
import BooksQueryForm from './Components/BooksQueryForm';
import LoginForm from './Components/LoginForm';
import NavigationBar from './Components/NavigationBar';
import { logDOM } from '@testing-library/react';

function App() {
  const [loggedIn, setLoggedIn] = useState(false);
  const loggedInState = {loggedIn, setLoggedIn}


  return (
    <UserLoginStateContext.Provider value={loggedInState}>
      <NavigationBar/>
      <div className="content-container">
      {loggedIn? <BooksQueryForm/>: <LoginForm />}
      </div>
    </UserLoginStateContext.Provider>
  )
}

export default App;
