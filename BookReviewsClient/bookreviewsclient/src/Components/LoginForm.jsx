import React from "react";
import UserLoginStateContext from "./UserLoginStateContext";
import { useState, useContext } from "react";
import {Navigate, Link} from "react-router-dom"
import {config} from "../Constants"

export default function LoginForm()
{
    const {user,setLoggedIn}= useContext(UserLoginStateContext);
    const [failedToLogin, setFailedToLogin] = useState(false);

    async function LoginUser(e)
    {
        e.preventDefault();
        const form = e.target;
        const formData = new FormData(form);

        const userName = formData.get("login");
        const password = formData.get("password");
        try{
            const response = await fetch(`${config.url}/api/v1/login`,{
                                    method: "POST",
                                    credentials: "include",
                                    body:JSON.stringify({userName, password}),
                                    headers: {
                                        "Content-Type": "application/json",
                                    }
            })  

            if(!response.ok)
            {
                console.error("Couldn't login now, try again later");
                setFailedToLogin(true);
                return;
            }

            const data = await response.json();
            setLoggedIn({
                userName: data.userName,
                id: data.id
            })
            setFailedToLogin(false);
            
        }catch(error)
        {
            console.error("Couldn't login now, try again later");
            setFailedToLogin(true);
            return;
        }
    }
    console.log(user);

    return (
        <div className="form-container">
        {user?
        <Navigate from="/login" to="/profile"/>:
        <form onSubmit={LoginUser} method="post" className="app-form">
            <input type="text" name="login" placeholder="username" className="login-input"/>
            <input type="password" name="password" placeholder="•••••••••" id="password-input" className="login-input"/>
            <button className="form-button" >Log In</button>
        </form>
        }
        {failedToLogin?<p style={{"color": "red"}}>Failed to login, check your login and password and try again.</p>:null}
        <Link to="/registration">
            <p>Register</p>
        </Link>
        </div>      
    );
}