-- Migration date: 25.11.2025
-- Add UserSettings.PreferredCurrency column
-- Add CurrencyRatesCache table
-- Add Cars table

ALTER TABLE [UserSettings]
ADD COLUMN [PreferredCurrency] TEXT NOT NULL DEFAULT 'EUR';

CREATE TABLE IF NOT EXISTS [CurrencyRatesCache] (
    [Id] INTEGER PRIMARY KEY,
    [FromCurrency] TEXT NOT NULL,
    [ToCurrency] TEXT NOT NULL,
    [Rate] NUMERIC NOT NULL,
    [Date] TEXT NOT NULL,
    [CachedAt] TEXT NOT NULL DEFAULT (DATETIME('now'))
);

CREATE TABLE IF NOT EXISTS [Cars] (
    [Id] TEXT PRIMARY KEY,
    [UserId] TEXT,
    [Manufacturer] TEXT NOT NULL,
    [Model] TEXT NOT NULL,
    [Specification] TEXT NOT NULL,
    [ModelYear] INTEGER NOT NULL,
    [Image] BLOB,

    FOREIGN KEY ([UserId]) REFERENCES [Users]([Id])
);