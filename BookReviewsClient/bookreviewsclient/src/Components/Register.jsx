import { useState } from "react";
import { Navigate } from "react-router-dom";
import {config} from "../Constants"

const RegistrationForm = ()=>
{
    const [registered, setRegisterd] = useState(false);
    async function registerUser(e)
    {
        e.preventDefault();

        const form = e.target;
        const formData = new FormData(form);

        const userName = formData.get("login");
        const password = formData.get("password");
        const repeatedPassword = formData.get("repeated-password");

        if(password!==repeatedPassword)
        {
            alert("passwords don't match");
            return;
        }

        await fetch(`${config.url}/api/v1/register`,{
                                method: "POST",
                                credentials: "include",
                                body:JSON.stringify({userName, password}),
                                headers: {
                                    "Content-Type": "application/json",
                                }
        }).then((response)=>
        {
            e.preventDefault();
            if(response.status === 200)
            {
                setRegisterd(true);
                console.log(userName);
            }
        })
    }

    return (
        registered?
        <Navigate to="/login"/>:
        <div className="form-container">
            <form onSubmit={registerUser} method="post" className="app-form">
                <input type="text" name="login" placeholder="username" className="app-input" required/>
                <input type="password" name="password" placeholder="password" className="app-input" required/>
                <input type="password" name="repeated-password" placeholder="repeat password" className="app-input" required/>
                <button className="form-button" >Register</button>
            </form>
        </div>

    )
}

export default RegistrationForm;