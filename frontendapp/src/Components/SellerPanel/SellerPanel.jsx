import React, { useEffect, useState } from 'react'
import Grid from '@mui/material/Grid';
import { getSellerArticlesAction, deleteArticleAction } from '../../Store/articleSlice';
import { useDispatch, useSelector } from 'react-redux';
import Navbar from '../Navbar/Navbar';
import { Paper, Button, Avatar } from '@mui/material';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Title from '../AdminPanel/Title/Title';
import UpdateModal from './Modal/UpdateModal';
import Box from '@mui/material/Box';
import NewArticleModal from './Modal/NewArticleModal';

export default function SellerPanel() {
    const dispatch = useDispatch();
    const articles = useSelector((state) => state.article.sellerArticles);
    const [isInitial, setIsInitial] = useState(true);
    const img = "data:image/*;base64,";
    const imgUrl = process.env.PUBLIC_URL + '/no-image.jpg';
    const [modalOpen, setModalOpen] = useState(false);
    const [selectedArticle, setSelectedArticle] = useState(null);

    const [modalNewOpen, setModalNewOpen] = useState(false);

    const handleCloseModal = () => {
        setModalOpen(false);
        const execute =  () => {
             dispatch(getSellerArticlesAction());
        };
       execute();
    };

    const handleCloseNewModal = () => {
        setModalNewOpen(false);
        const execute =  () => {
             dispatch(getSellerArticlesAction());
        };
       execute();
    };
    const handleUpdateArticle = (article) => {
       
        setSelectedArticle(article);
        setModalOpen(true);
    };
    const handleAddProduct =() =>{
        setModalNewOpen(true);
    }
    const handleDeleteArticle = (articleId) => {
        const data = new FormData();
        data.append('id', articleId);
        const deleteArticle =  () => {
            dispatch(deleteArticleAction(data)).then(() => {
            try {
                dispatch(getSellerArticlesAction());
              } catch (error) {
                console.error('Delete article error: ', error);
              }
            });
        }
        deleteArticle();

    }
    useEffect(() => {
        if (!isInitial) {
            return;
        }
       
        const execute = () => {
            dispatch(getSellerArticlesAction());
        };
       execute(); 
        setIsInitial(false);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [isInitial]);


    if (isInitial) {
        return <div>Loading...</div>
    } else
        return (
            <React.Fragment>
                <Navbar />
                <Grid container justifyContent="center" mt={4}>
                    <Grid item xs={12} sm={8} md={6} lg={10}>
                        <Paper sx={{ p: 2, display: 'flex', flexDirection: 'column' }}>
                        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                    <Title >MY PRODUCTS</Title>
                    <Button
                        variant="contained"
                        color="secondary"
                        className="addButton"
                        onClick={() => handleAddProduct()}
                        sx={{mt:0}}
                    > 
                        Add New Product
                    </Button>
                    <NewArticleModal open={modalNewOpen} onClose={handleCloseNewModal} />
                </Box>
                            <Table size="medium" >
                                <TableHead >
                                    <TableRow >
                                        <TableCell sx={{ fontSize: 20, fontWeight: 'bold' }} >Image</TableCell>
                                        <TableCell sx={{ fontSize: 20, fontWeight: 'bold' }} >Name</TableCell>
                                        <TableCell sx={{ fontSize: 20, fontWeight: 'bold' }}>Description</TableCell>
                                        <TableCell sx={{ fontSize: 20, fontWeight: 'bold' }}>Price</TableCell>
                                        <TableCell sx={{ fontSize: 20, fontWeight: 'bold' }}>Max Quantity</TableCell>
                                        <TableCell></TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {articles.map((article) => (
                                        <TableRow key={article.id} >
                                            <TableCell style={{ width: '20%', height: '20%' }}>
                                                <Avatar
                                                    src={article.photoUrl !== "" ? img + article.photoUrl : imgUrl}
                                                    style={{ width: '100%', height: 'auto' }}
                                                >
                                                </Avatar>
                                            </TableCell>
                                            <TableCell>{article.name}</TableCell>
                                            <TableCell>{article.description}</TableCell>
                                            <TableCell>{article.price}</TableCell>
                                            <TableCell>{article.maxQuantity}</TableCell>
                                            <TableCell align="right">
                                                <Button
                                                    variant="contained"
                                                    color="primary"
                                                    className="approveButton"
                                                    onClick={() => handleUpdateArticle(article)}
                                                >
                                                    Update
                                                </Button>
                                                <Button
                                                    sx={{ ml: 2 }}
                                                    variant="contained"
                                                    color="error"
                                                    onClick={() => handleDeleteArticle(article.id)}
                                                >
                                                    Delete
                                                </Button>
                                                {selectedArticle && (
                                                    <UpdateModal open={modalOpen} onClose={handleCloseModal} article={selectedArticle} />
                                                )}
                                            </TableCell>
                                        </TableRow>
                                    ))}
                                </TableBody>
                            </Table>
                        </Paper>
                    </Grid>
                </Grid>

            </React.Fragment>
        )
}