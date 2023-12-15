import React from "react";
import UserLoginStateContext from "./UserLoginStateContext";
import { useContext } from "react";
import {Navigate, Link} from "react-router-dom"
import {config} from "../Constants"

export default function LoginForm()
{
    const {user,setLoggedIn}= useContext(UserLoginStateContext);

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
                return;
            }
            
            const data = await response.json();
            setLoggedIn({
                userName: data.userName,
                id: data.id
            })
            
        }catch(error)
        {
            console.error("Couldn't login now, try again later");
            return;
        }
    }
    console.log(user);

    return (
        <div className="form-container">
        {user?
        <Navigate from="/login" to="/"/>:
        <form onSubmit={LoginUser} method="post" className="app-form">
            <input type="text" name="login" placeholder="username" className="login-input"/>
            <input type="password" name="password" placeholder="•••••••••" id="password-input" className="login-input"/>
            <button className="form-button" >Log In</button>
        </form>
        }
        <Link to="/registration">
            <p>Register</p>
        </Link>
        </div>      
    );
}