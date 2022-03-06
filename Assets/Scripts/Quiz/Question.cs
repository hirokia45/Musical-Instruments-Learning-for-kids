using System;
using System.Collections.Generic;
using System.Linq;

public class Question
{
    public InstrumentSO CorrectInstrument { get; set; }
    public List<InstrumentSO> Options { get; set; } = new List<InstrumentSO>();
    public int AnswerIndex => Options.FindIndex(item => item.InstrumentType == CorrectInstrument.InstrumentType);

    public Question(InstrumentSO correctInstrument)
    {
        CorrectInstrument = correctInstrument;
        Options.Add(correctInstrument);
    }

    public void AddOption(InstrumentSO option)
    {
        Options.Add(option);
    }

    public void Shuffle()
    {
        Options = Options.OrderBy(a => Guid.NewGuid()).ToList();
    }
}