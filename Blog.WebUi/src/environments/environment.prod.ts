export const environment = {
    env: "prod",
    baseApiUrl: "api",
    roles: {
        admin: 'admin',
        moderator: 'moderator',
    },
    passwordPattern: /^(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[^\w\d\s:])([^\s]){8,}$/
}