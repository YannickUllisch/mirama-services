namespace Mirama.Modules.Clients.Domain.Aggregates.IntakeForm;

// "Show/require this field only if field X satisfies operator/value" - the
// referenced field is by Key, not by object reference, so a condition
// survives a ReviseFields round-trip as long as the target Key still exists.
// IntakeForm.Create/ReviseFields is what checks the Key actually resolves
// and doesn't form a cycle; this record only holds the shape.
public sealed record IntakeFieldCondition
{
    public string DependsOnFieldKey { get; }
    public IntakeFieldConditionOperator Operator { get; }
    public string? Value { get; }

    private IntakeFieldCondition(string dependsOnFieldKey, IntakeFieldConditionOperator @operator, string? value)
    {
        DependsOnFieldKey = dependsOnFieldKey;
        Operator = @operator;
        Value = value;
    }

    public static IntakeFieldCondition Create(string dependsOnFieldKey, IntakeFieldConditionOperator @operator, string? value = null)
    {
        if (string.IsNullOrWhiteSpace(dependsOnFieldKey))
            throw new ArgumentException("A condition must depend on a field.", nameof(dependsOnFieldKey));

        if ((@operator is IntakeFieldConditionOperator.Equals or IntakeFieldConditionOperator.NotEquals or IntakeFieldConditionOperator.Contains)
            && string.IsNullOrEmpty(value))
            throw new ArgumentException($"Operator {@operator} requires a comparison value.", nameof(value));

        return new IntakeFieldCondition(dependsOnFieldKey.Trim(), @operator, value);
    }

    public bool IsSatisfiedBy(IReadOnlyDictionary<string, string> responses)
    {
        var hasAnswer = responses.TryGetValue(DependsOnFieldKey, out var answer) && !string.IsNullOrEmpty(answer);

        return Operator switch
        {
            IntakeFieldConditionOperator.IsAnswered => hasAnswer,
            IntakeFieldConditionOperator.IsNotAnswered => !hasAnswer,
            IntakeFieldConditionOperator.Equals => hasAnswer && string.Equals(answer, Value, StringComparison.OrdinalIgnoreCase),
            IntakeFieldConditionOperator.NotEquals => !hasAnswer || !string.Equals(answer, Value, StringComparison.OrdinalIgnoreCase),
            IntakeFieldConditionOperator.Contains => hasAnswer && answer!.Contains(Value!, StringComparison.OrdinalIgnoreCase),
            _ => throw new InvalidOperationException($"Unhandled condition operator {Operator}.")
        };
    }
}
