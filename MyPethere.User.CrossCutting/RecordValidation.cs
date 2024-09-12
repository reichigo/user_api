namespace MyPethere.User.CrossCutting
{
    public abstract record RecordValidation
    {
        protected RecordValidation()
        {
            Validate();
        }

        protected virtual void Validate()
        {
        }
    }
}
