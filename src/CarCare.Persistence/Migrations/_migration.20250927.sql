-- Migration date: 27.09.2025
-- Add Users table
-- Add UserSettings table

CREATE TABLE IF NOT EXISTS [Users] (
    [Id] TEXT PRIMARY KEY,
    [Username] TEXT NOT NULL UNIQUE,
    [Password] TEXT NOT NULL,
    [Avatar] BLOB
);

CREATE TABLE IF NOT EXISTS [UserSettings] (
    [UserId] TEXT PRIMARY KEY,
    [Theme] TEXT,

    FOREIGN KEY ([UserId]) REFERENCES [Users]([Id])
);