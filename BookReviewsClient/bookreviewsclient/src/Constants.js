const production = 
{
    url: 'https://bookreviewsapi.azurewebsites.net/'
}

const development = {
    url: 'https://localhost:7235'
}

export const config = process.env.NODE_ENV === 'production'? production: development;