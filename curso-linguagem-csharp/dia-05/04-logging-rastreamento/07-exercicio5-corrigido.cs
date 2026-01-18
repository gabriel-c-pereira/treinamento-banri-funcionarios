// ❌ Ineficiente
Log.Debug("Processando {@Registro}", registro); // Sempre serializa

// ✅ Otimizado
if (Log.IsEnabled(LogEventLevel.Debug))
{
    Log.Debug("Processando {@Registro}", registro);
}

// ✅ Sampling (1%)
if (Random.Shared.Next(100) < 1)
{
    Log.Debug("Processando {@Registro}", registro);
}