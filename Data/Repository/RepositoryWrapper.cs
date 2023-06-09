﻿using Feipder.Entities;

namespace Feipder.Data.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private DataContext _context;
        private IProductRepository _products;
        private ICategoryRepository _categories;
        private ISizeRepository _sizes;
        private IProductStorageRepository _storage;
        private IColorRepository _colors;

        private ImageRepository _images;

        public ImageRepository Images
        {
            get
            {
                if(_images == null)
                {
                    _images = new ImageRepository();
                }

                return _images;
            }
        }

        public IColorRepository Colors
        {
            get
            {
                if(_colors == null)
                {
                    _colors = new ColorRepository(_context);
                }

                return _colors;
            }
        }

        public IProductRepository Products {
            get {

                if (_products == null)
                {
                    _products = new ProductRepository(_context);
                }

                return _products;
            }
        }

        public ICategoryRepository Categories
        {
            get
            {
                if (_categories == null)
                {
                    _categories = new CategoryRepository(_context);
                }

                return _categories;
            }
        }

        public ISizeRepository Sizes {
            get
            {
                if(_sizes == null)
                {
                    _sizes = new SizeRepository(_context);
                }

                return _sizes;
            }
        }

        public IProductStorageRepository Storage
        {
            get
            {
                if(_storage == null)
                {
                    _storage = new ProductStorageRepository(_context);
                }

                return _storage;
            }
        }

        public RepositoryWrapper(DataContext context)
        {
            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
