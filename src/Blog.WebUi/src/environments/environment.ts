export const environment = {
    baseApiUrl: 'http://localhost:3000/api',
    roles: {
        admin: 'admin',
        moderator: 'moderator',
    },
    passwordPattern: /^(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[^\w\d\s:])([^\s]){8,}$/
}