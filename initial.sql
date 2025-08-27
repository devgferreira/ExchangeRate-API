
CREATE TABLE Users (
    Id SERIAL PRIMARY KEY,          -- Identificador único, auto-incremento
    Name VARCHAR(255) NOT NULL,     -- Nome do usuário
    Email VARCHAR(255) NOT NULL UNIQUE, -- Email único
    PasswordHash BYTEA NOT NULL,    -- Hash da senha
    PasswordSalt BYTEA NOT NULL,    -- Salt da senha
    CreatedAt TIMESTAMP NOT NULL DEFAULT NOW() -- Data de criação
);

CREATE TABLE Currency (
    code TEXT NOT NULL,
    codein TEXT NOT NULL,
    bid NUMERIC(18,6) NOT NULL,
    ask NUMERIC(18,6) NOT NULL,
    DateOfCurrency DATE NOT NULL,
    CreatedAT TIMESTAMP NOT NULL DEFAULT NOW(),
    CONSTRAINT unique_currency_date UNIQUE (DateOfCurrency)
);