using System;

public interface IHaveHealthUI
{ 
    event Action<float,float> OnHealthChanged;
}
