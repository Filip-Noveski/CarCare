-- Migration date: 25.11.2025
-- Add UserSettings.PreferredCurrency column
-- Add CurrencyRatesCache table

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