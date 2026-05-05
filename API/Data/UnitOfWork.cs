using API.Data.Repositories;
using API.Interfaces;
using API.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;

    public UnitOfWork(DataContext context)
    {
        _context = context;
    }

    private IMessageRepository? _messageRepository;
    private IMemberRepository? _memberRepository;
    private ILikesRepository? _likesRepository;
    private IPhotoRepository? _photoRepository;

    public IMemberRepository MemberRepository => _memberRepository ??= new MemberRepository(_context);

    public IMessageRepository MessageRepository => _messageRepository ??= new MessageRepository(_context);

    public ILikesRepository LikesRepository => _likesRepository ??= new LikesRepository(_context);

    public IPhotoRepository PhotoRepository => _photoRepository ??= new PhotoRepository(_context);

    public async Task<bool> Complete()
    {
        try
        {
            return await _context.SaveChangesAsync() > 0;
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("An exception ocurred while saving changes.", ex);
        }
    }

    public bool HasChanges()
    {
        return _context.ChangeTracker.HasChanges();
    }
}
