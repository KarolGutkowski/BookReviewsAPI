import { createContext } from "react";

const UserLoginStateContext = createContext(
    {
        user: null,
        setLoggedIn: ()=> {}
    });

export default UserLoginStateContext;
