export const environment = {
    env: "prod",
    baseApiUrl: "https://blog-api.146.235.60.141.sslip.io/api",
    roles: {
        admin: 'admin',
        moderator: 'moderator',
    },
    passwordPattern: /^(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[^\w\d\s:])([^\s]){8,}$/
}