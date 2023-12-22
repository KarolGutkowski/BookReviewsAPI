import { withErrorBoundary , useErrorBoundary } from "react-use-error-boundary";

const ErrorBoundary = withErrorBoundary(({children})=>
{
    // eslint-disable-next-line no-unused-vars
    const [error, _] = useErrorBoundary((error, errorInfo)=>
    {
        console.error("An error occured");
    });

    if(error){
        return <p>Error</p>
    }
    return <>{children}</>;
})

export default ErrorBoundary;